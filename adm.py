import os
import os.path
import time

os.system('cls')
os.system('title admin')

helpSet = '''
username
set
clear

theme
console
help

lock
format
exit
'''
class admin():
	def aosadmin():
		value = True
		while value:
			admin = input("$")
			if "username" in admin.lower():
				os.system('echo %username%')

			elif "set" in admin.lower():
				os.system('call Packages/data/oza/admin.bat')
				print("log.set saved.")

			elif "clear" in admin.lower():
				os.system('cls')
				print("Cleaned.")
				time.sleep(1)
				os.system('cls')

			elif "theme" in admin.lower():
				os.system('color f0')
				print("Theme changed.")
				time.sleep(1)
				os.system('cls')

			elif "console" in admin.lower():
				os.system('cls')
				os.system('call cmd.exe')

			elif "exit" in admin.lower():
				break

			elif "help" in admin.lower():
				print(helpSet)

			elif "lock" in admin.lower():
				global passwork
				lock = input()
				passwork = lock
				if lock == "" or lock == " " or '  ' in lock:
					os.system('if exist user.set==del user.set')
					print("Password deleted.")
					time.sleep(1)
					os.system('cls')

				else:
					os.system('echo|set /p = ' + '"' + passwork + '"' + '>>user.set')
					print("System got secured.")
					time.sleep(1)
					os.system('cls')

			elif "format" in admin.lower():
				print("Formatting...")
				time.sleep(3)
				os.system('cls')
				print("Formatting completed.")
				time.sleep(1)
				os.system('cls')

			elif "read" in admin.lower():
				file = input('''Please enter your file name.
''')
				fetch = os.path.isfile("log/" + file)
				if fetch == False:
					input("File does not exist.")

				else:
					reading = open("log/" + file, "r")
					sat = reading.read()
					os.system('cls')
					print("__________ " + file + " __________")
					print(sat)
					input("Continue.")
					os.system('cls')

			else:
				print(admin)

if __name__ == '__main__':
	admin.aosadmin()
