import datetime
from datetime import timezone
import jwt
JWT_SECRET = "6aa50cfdacdc3f804cbcfa2a8855246b254852ed27d5acfc296ec572de135454"


def jwt_generate(user_id) -> str:
    if not isinstance(user_id, dict):
        user_id = user_id.dict()
    user_id["exp"] = datetime.datetime.now(tz=timezone.utc) + datetime.timedelta(hours=2)
    return jwt.encode(user_id, JWT_SECRET, algorithm="HS256")


def jwt_validate(token) -> bool:
    try:
        jwt.decode(token, JWT_SECRET, algorithms="HS256", options={"verify_exp": False})
        return True
    except jwt.exceptions.DecodeError:
        return False


def jwt_update(token):
    old_token = jwt.decode(
        token, JWT_SECRET, algorithms="HS256", options={"verify_exp": False}
    )
    old_token.pop("exp")
    new_token = jwt_generate(old_token)
    return new_token


def jwt_user_id(token) -> int:
    user = jwt.decode(
        token, JWT_SECRET, algorithms="HS256", options={"verify_exp": False}
    )
    return int(user["user_id"])
