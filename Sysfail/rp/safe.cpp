// RESTORE all files from RECOVERY folder.
#include <filesystem>
#include <iostream>
#include <ctime>

#include <windows.h>

using namespace std;

int main(int argc, char const *argv[])
{
	string command = "";
	if (string(argv[1]) == "-r")
	{
		string recovery_path = string(argv[2]) + "\\Sysfail\\RECOVERY";
		command = "robocopy \"" + recovery_path + "\" \"" + string(argv[2]) + "\" /E /S /NFL /NDL /NJH /NJS /nc /ns /np";

		system(command.c_str());
	}

	else if (string(argv[1]) == "-p")
	{
		time_t t = time(0);

		// Get current date and time in desired format
		char date_time[20];
		strftime(date_time, sizeof(date_time), "%d-%m-%Y %H-%M-%S", localtime(&t));

		// Create directory with current date and time as name
		string rp_dir = string(argv[2]) + "\\SoftwareDistribution\\RestorePoint\\" + string(date_time); // rp_dir -> Restore Point Directory.
		filesystem::create_directory(rp_dir);

		string exclude_dir = string(argv[2]) + "\\SoftwareDistribution";
		command = "robocopy \"" + string(argv[2]) + "\" \"" + rp_dir + "\" /XD \"" + exclude_dir + "\" /E /S /NFL /NDL /NJH /NJS /nc /ns /np";

		system(command.c_str());
	}

	else
	{
		cout << "Use -r to restore AOs" << endl;
		cout << "Use -p to create a restore point of AOs" << endl;
	}

    return 0;
}
