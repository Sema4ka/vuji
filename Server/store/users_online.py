from store.models.models import *
import store.user as user_store
from datetime import datetime

USER_CAN_BE_ONLINE = 10


def get_user_online_by_user_id(session, user_id):
    user_online = session.query(UsersOnline).filter_by(user_id=user_id).one()
    return user_online


def user_is_online(session, user_id) -> bool:
    current_time = int(datetime.now().timestamp())
    user_online = get_user_online_by_user_id(session, user_id)
    if user_online.last_online + USER_CAN_BE_ONLINE >= current_time:
        return True
    return False


def create_user_online(session, user_id):
    user = UsersOnline(user_id=user_id, last_online=0)
    session.add(user)
    session.commit()


def change_user_online(session, user_id):
    user_online = get_user_online_by_user_id(session, user_id)
    user_online.last_online = int(datetime.now().timestamp())
    session.add(user_online)
    session.commit()


def change_user_offline(session, user_id):
    user_online = get_user_online_by_user_id(session, user_id)
    user_online.last_online = 0
    session.add(user_online)
    session.commit()
