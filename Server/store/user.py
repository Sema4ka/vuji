import store
from store.models.models import *
import random
import datetime

def get_user(session, **kwargs):
    try:
        user = session.query(User).filter_by(**kwargs).one()
        return user
    except:
        return None


def create_user(session, login, password):
    created_at = datetime.datetime.now()
    bcrypt_pass = bcrypt.hashpw(password.encode("utf-8"), bcrypt.gensalt()).decode("utf-8")
    user = User(login=login, password=bcrypt_pass, created_at=created_at,
                username="user_" + str(random.randint(1000000, 10000000)))
    try:
        session.add(user)
        session.commit()
        user = session.query(User).filter_by(login=login).one()
        return user
    except:
        return None


def login(session, login, password):
    user = get_user(session, login=login)
    if user is None:
        return None
    if not bcrypt.checkpw(password.encode("utf-8"), user.password.encode("utf-8")):
        return None
    return user
