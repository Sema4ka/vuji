import shutil
import os

dir_from = ""
dir_to = ""


def create_and_open():
    for i in range(1, 4):
        cp_to_dir = dir_to + f"\\{i}"
        shutil.copytree(dir_from, cp_to_dir)
        #os.system(f"start {cp_to_dir}\\Vuji.exe")


def just_open():
    for i in range(1, 4):
        dir = dir_to + f"\\{i}"
        print(f" start : {dir}")
        os.system(f"start {dir}\\Vuji.exe")



create_and_open()