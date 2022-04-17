import store
from store.models.models import *
import random
import datetime
import sqlalchemy.exc


def get_user(session, **kwargs):
    try:
        user = session.query(User).filter_by(**kwargs).one()
        return user
    except sqlalchemy.exc.NoResultFound:
        return None


def create_user(session, user_login, password):
    created_at = datetime.datetime.now()
    bcrypt_pass = bcrypt.hashpw(password.encode("utf-8"), bcrypt.gensalt()).decode("utf-8")
    # username = "user_" + str(random.randint(100000, 10000000))
    username = "user_" + str(user_login)
    user = User(login=user_login, password=bcrypt_pass, created_at=created_at,
                username=username)
    try:
        session.add(user)
        session.commit()
        user = session.query(User).filter_by(login=user_login).one()
        return user
    except sqlalchemy.exc.IntegrityError:
        return None


def login(session, user_login, password):
    user = get_user(session, login=user_login)
    if user is None:
        return None
    if not bcrypt.checkpw(password.encode("utf-8"), user.password.encode("utf-8")):
        return None
    return user


def find_users_by_name(session, name):
    users = session.query(User).filter(User.username.like(f"%{name}%"))
    user_list = [i for i in users]
    return user_list
