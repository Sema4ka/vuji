from typing import Optional
from pydantic import BaseModel


class RegisterBase(BaseModel):
    login: str
    password: str


class LoginBase(BaseModel):
    login: str
    password: str


class FindFriendsByNameBase(BaseModel):
    friendsName: str
