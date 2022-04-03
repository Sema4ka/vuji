from fastapi import FastAPI, Depends, Form, Request
from fastapi.security import OAuth2PasswordBearer, OAuth2PasswordRequestForm
import uvicorn
from store import session_factory
from starlette.responses import Response
import JWT_func as jwt_func
from sqlalchemy.orm import Session
from store import user as user_store, users_online as users_online_store
from schemas import *
import json

app = FastAPI()

oauth2_scheme = OAuth2PasswordBearer(tokenUrl='token')


def get_session():
    db = session_factory()
    try:
        yield db
    finally:
        db.close()


def dict_to_json(**kwargs):
    json_string = json.dumps(kwargs)
    return json_string


@app.get('/')
def index():
    return "server online"


@app.post('/auth')
def auth(request: Request, session: Session = Depends(get_session)):
    token = request.headers['Authorization']
    if jwt_func.jwt_validate(token):
        user_id = jwt_func.jwt_user_id(token)
        user = user_store.get_user(session, id=user_id)
        if user is not None:
            if not users_online_store.user_is_online(session, user.id):
                updated_token = jwt_func.jwt_update(token)
                users_online_store.change_user_online(session, user.id)
                data = dict_to_json(token=updated_token)
                return Response(data, status_code=200)
    return Response("WrongToken", status_code=400)


@app.post('/register')
def register(body: RegisterBase, session: Session = Depends(get_session)):
    user = user_store.create_user(session, body.login, body.password)
    if user is not None:
        users_online_store.create_user_online(session, user.id)
        token = jwt_func.jwt_generate({"user_id": user.id})
        data = dict_to_json(token=token)
        return Response(data, status_code=200)
    else:
        return Response(status_code=400)


@app.post("/login")
def post_login(body: LoginBase, session: Session = Depends(get_session)):
    user = user_store.login(session, body.login, body.password)
    if user is not None:
        if not users_online_store.user_is_online(session, user.id):
            users_online_store.change_user_online(session, user.id)
            token = jwt_func.jwt_generate({"user_id": user.id})
            data = dict_to_json(token=token)
            return Response(data, status_code=200)
    return Response(status_code=400)


@app.put("/user_online")
def put_user_online(request: Request, session: Session = Depends(get_session)):
    token = request.headers['Authorization']
    if jwt_func.jwt_validate(token):
        user_id = jwt_func.jwt_user_id(token)
        users_online_store.change_user_online(session, user_id)
        return Response(status_code=200)
    return Response(status_code=400)


@app.put("/user_offline")
def put_user_offline(request: Request, session: Session = Depends(get_session)):
    token = request.headers['Authorization']
    if jwt_func.jwt_validate(token):
        user_id = jwt_func.jwt_user_id(token)
        users_online_store.change_user_offline(session, user_id)
        return Response(status_code=200)
    return Response(status_code=400)


@app.get("/user_id")
def get_user_id(request: Request):
    token = request.headers['Authorization']
    if jwt_func.jwt_validate(token):
        user_id = str(jwt_func.jwt_user_id(token))
        data = dict_to_json(userID=user_id)
        return Response(data, status_code=200)
    return Response(status_code=400)


if __name__ == "__main__":
    uvicorn.run(app, host="127.0.0.1", port=8000, timeout_keep_alive=0)
