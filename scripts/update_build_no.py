import glob, os

os.chdir("..")
files = glob.glob('[0-9]*') # Get a list of all files in the current directory with names consisting only of digits
file_with_numbers_name = next(filter(lambda f: not os.path.isdir(f), files), None) # Filter the list to only include files (not directories) and pick the first one

# If a file with a name consisting only of digits exists in the current directory
if file_with_numbers_name:
    new_name = str(int(file_with_numbers_name) + 1)
    os.rename(file_with_numbers_name, new_name)

else:
    print("No build no. file found.")
