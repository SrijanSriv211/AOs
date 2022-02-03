import os
class safe():
    def locker():
        os.system('cls')
        check = open('user.set', 'r')
        line = check.readlines()
        passlock = input("Password: ")
        if passlock == line[0]:
            input("Boot.")

        else:
            safe.Restart()

    def Restart():
        safe.locker()

if __name__ == '__main__':
    safe.locker()
