from sqlalchemy.ext.declarative import declarative_base
import bcrypt
from sqlalchemy import Column, Integer, String, DATETIME, ForeignKey
from sqlalchemy.orm import relationship
from sqlalchemy.sql.schema import Table
from datetime import datetime

Base = declarative_base()


class User(Base):
    __tablename__ = "users"

    id = Column(Integer, primary_key=True)
    login = Column(String, unique=True, nullable=False)
    username = Column(String, unique=False, nullable=False)
    password = Column(String)
    created_at = Column(DATETIME)
    users_online = relationship("UsersOnline", backref="users_online", cascade="all,delete,save-update")

    def __repr__(self):
        return (
            f"<USER ID: {self.id}, login: {self.login}, created_at: {self.created_at}>"
        )

    def change(self, session, **kwargs):
        for i, value in kwargs.items():
            if i in self.__dict__:
                attr = self.__getattribute__(i)
                self.__setattr__(i, value if value else attr)
        session.add(self)
        session.commit()


class UsersOnline(Base):
    __tablename__ = "users online"
    id = Column(Integer, primary_key=True)
    user_id = Column(Integer, ForeignKey("users.id"), unique=True)
    last_online = Column(Integer)


