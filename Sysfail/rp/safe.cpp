// RESTORE all files from RECOVERY folder.
#include <filesystem>
#include <iostream>
#include <ctime>

#include <windows.h>

using namespace std;

int main(int argc, char const *argv[])
{
	string command = "";
    if (argc < 2)
	{
        cout << "Invalid number of arguments." << endl;
        return 1;
    }

	else if (string(argv[1]) == "-r")
	{
		string aos_path = argv[2];
		string recovery_path = aos_path + "\\Sysfail\\RECOVERY";

        cout << "Restoring all files from the recovery folder..." << endl;
        cout << "Source: " << recovery_path << endl;
        cout << "Destination: " << aos_path << endl;

		command = "robocopy \"" + recovery_path + "\" \"" + aos_path + "\" /e /s /nfl /ndl /njh /njs /nc /ns /np";
		system(command.c_str());
	}

	else if (string(argv[1]) == "-p")
	{
		time_t t = time(0);

		// Get current date and time in desired format
		char date_time[20];
		strftime(date_time, sizeof(date_time), "%d-%m-%Y %H-%M-%S", localtime(&t));

		// Create directory with current date and time as name
		string aos_path = argv[2];
		string rp_dir = aos_path + "\\SoftwareDistribution\\RestorePoint\\" + string(date_time); // rp_dir -> Restore Point Directory.
		string exclude_dir = aos_path + "\\SoftwareDistribution";

		filesystem::create_directory(rp_dir);

        cout << "Creating a restore point all files to recovery folder..." << endl;
        cout << "Source: " << aos_path << endl;
        cout << "Destination: " << rp_dir << endl;

		command = "robocopy \"" + aos_path + "\" \"" + rp_dir + "\" /XD \"" + exclude_dir + "\" /e /s /nfl /ndl /njh /njs /nc /ns /np";
		system(command.c_str());
	}

	else
	{
		cout << "Use -r to restore AOs" << endl;
		cout << "Use -p to create a restore point of AOs" << endl;
	}

    return 0;
}
