"""Fast API сервер"""
import json
from fastapi import FastAPI, Depends, Request
from fastapi.security import OAuth2PasswordBearer
import uvicorn
from starlette.responses import Response
from sqlalchemy.orm import Session
import schemas
from store import session_factory
import JWT_func as jwt_func

from store import user as user_store, users_online as users_online_store

app = FastAPI()

oauth2_scheme = OAuth2PasswordBearer(tokenUrl='token')


def get_session():
    """
    получение сессии
    :return: сессия
    """
    database = session_factory()
    try:
        yield database
    finally:
        database.close()


def dict_to_json(**kwargs):
    """
    словарь в json формат
    :param kwargs: данные
    :return: json строка
    """

    json_string = json.dumps(kwargs)
    return json_string


def array_to_json(arr):
    """
    массив в json формат
    :param arr: данные
    :return: json строка
    """
    json_string = json.dumps(arr)
    return json_string


def generate_info_about_user(user):
    """
    Информация о пользователе
    :param user: пользователь
    :return: словарь
    """
    res = {
        "userID": user.id,
        "username": user.username,
    }
    return res


@app.get('/')
def index():
    """
    index страница
    :return: string
    """
    return "server online"


@app.post('/auth')
def auth(request: Request, session: Session = Depends(get_session)):
    """
    Авто авторизация по токену пользоваетля
    :param request: request запрос
    :param session: сессия
    :return: response
    """
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
def register(body: schemas.RegisterBase, session: Session = Depends(get_session)):
    """
    Регистрация пользователя
    :param body: схема RegisterBase
    :param session: сессия
    :return: response
    """
    user = user_store.create_user(session, body.login, body.password)
    if user is not None:
        users_online_store.create_user_online(session, user.id)
        token = jwt_func.jwt_generate({"user_id": user.id})
        data = dict_to_json(token=token)
        return Response(data, status_code=200)
    return Response(status_code=400)


@app.post("/login")
def post_login(body: schemas.LoginBase, session: Session = Depends(get_session)):
    """
    Попытка авторизации
    :param body: LoginBase схема
    :param session: сессия
    :return: response
    """
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
    """
    PUT запрос на установку того что пользователь вошел в сеть
    :param request: request запрос
    :param session: сессия
    :return: response
    """
    token = request.headers['Authorization']
    if jwt_func.jwt_validate(token):
        user_id = jwt_func.jwt_user_id(token)
        users_online_store.change_user_online(session, user_id)
        return Response(status_code=200)
    return Response(status_code=400)


@app.put("/user_offline")
def put_user_offline(request: Request, session: Session = Depends(get_session)):
    """
    PUT запрос на установку того что пользователь вышел из сети
    :param request: request запрос
    :param session: сессия
    :return: response
    """
    token = request.headers['Authorization']
    if jwt_func.jwt_validate(token):
        user_id = jwt_func.jwt_user_id(token)
        users_online_store.change_user_offline(session, user_id)
        return Response(status_code=200)
    return Response(status_code=400)


@app.get("/user_id")
def get_user_id(request: Request):
    """
    Получения ID пользователя по токену
    :param request: request запрос
    :return: response
    """
    token = request.headers['Authorization']
    if jwt_func.jwt_validate(token):
        user_id = str(jwt_func.jwt_user_id(token))
        data = dict_to_json(userID=user_id)
        return Response(data, status_code=200)
    return Response(status_code=400)


@app.post("/find_friends_by_name")
def post_find_friends_by_name(body: schemas.FindFriendsByNameBase,
                              request: Request,
                              session: Session = Depends(get_session)):
    """
    Поиск пользователей по имени
    :param body: схема поиска
    :param request: request запрос
    :param session: сессия
    :return: response
    """
    token = request.headers['Authorization']
    name = body.friendsName

    if jwt_func.jwt_validate(token) and name != '':
        users_list = user_store.find_users_by_name(session, name)
        if users_list is not None:
            users_info = []
            for user in users_list:
                if user.id != jwt_func.jwt_user_id(token):
                    users_info.append(generate_info_about_user(user))
            data = array_to_json(users_info)
            return Response(data, status_code=200)
    return Response(status_code=400)


@app.get("/me")
def get_user_info(request: Request, session: Session = Depends(get_session)):
    """
    Получение имени пользователя

    Args:
        request (Request): Запрос на сервер
        session (Session): Сессия базы данных.
    Returns:
        Response:
            string Username: Имя пользователя
    """

    token = request.headers['Authorization']
    if jwt_func.jwt_validate(token):
        user = user_store.get_user(session, id=jwt_func.jwt_user_id(token))
        data = {
            "login": user.login,
            "username": user.username,
            "created_at": user.created_at
        }
        return Response(data, status_code=200)
    return Response(status_code=400)


def main():
    """
    Запуск сервера
    :return: None
    """
    uvicorn.run(app, host="127.0.0.1", port=8000, timeout_keep_alive=0)


if __name__ == '__main__':
    main()
