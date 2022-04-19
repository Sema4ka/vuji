from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker
from store.models.models import Base

engine = create_engine("sqlite:///vuji_server.db", connect_args={'check_same_thread': False})
session_factory = sessionmaker(engine)
Base.metadata.create_all(engine)
