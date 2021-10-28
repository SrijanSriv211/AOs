# Filer
# These imports will be used for this project.
import random
import string
import sys
import os

def EncryptFile(Filepath, Output_Filename):
	global Chars

	Output = []
	with open(Filepath, "r") as File:
		for Line in File:
			Line = Line.replace("\n", "")
			for Characters in Line:
				FindIndex = Chars[0].index(Characters)
				Output.append(Chars[1][FindIndex])

			Output.append("\n")

	with open(Output_Filename, "w") as File:
		for i in Output: File.writelines(i)

def DecryptFile(Filepath, Output_Filename):
	global Chars

	Output = []
	with open(Filepath, "r") as File:
		for Line in File:
			Line = Line.replace("\n", "")
			for Characters in Line:
				FindIndex = Chars[1].index(Characters)
				Output.append(Chars[0][FindIndex])

			Output.append("\n")

	with open(Output_Filename, "w") as File:
		for i in Output: File.writelines(i)

def Filer(Filepath, TaskToDO, KeySeed, Output_Filename):
	random.seed(KeySeed)

	# Split the List of these String Operations, and Join them to Chars List 2 Times.
	for i in range(2):
		Chars.append(list(string.ascii_letters + string.digits + string.punctuation))
	Chars[0].append(" ")
	Chars[1].append(" ")

	# Shuffle Chars[1] List.
	random.shuffle(Chars[1])

	if TaskToDO == "encrypt": EncryptFile(Filepath, Output_Filename)
	elif TaskToDO == "decrypt": DecryptFile(Filepath, Output_Filename)

def main():
	global Filepath, TaskToDO, KeySeed, OutputFile
	if os.path.isfile(Filepath):
		if KeySeed.isdigit():
			TaskToDO = TaskToDO.lower()
			Filer(Filepath, TaskToDO, KeySeed, OutputFile)

if __name__ == "__main__":
	Filepath = sys.argv[1]
	TaskToDO = sys.argv[2]
	KeySeed = sys.argv[3]
	OutputFile = sys.argv[4]
	Chars = []

	main()
