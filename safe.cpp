// RESTORE all files from RECOVERY folder.
#include <filesystem>
#include <windows.h>
#include <string.h>
#include <iostream>
#include <sstream>
#include <ctime>

using namespace std;

int main(int argc, char const *argv[])
{
	// TODO: Fix this code. It doesn't work.
	string command = "";
	if (string(argv[1]) == "-r")
	{
		command = "robocopy . \"" + filesystem::current_path().string() + "\\Sysfail\\RECOVERY\"" + " . /E /S /NFL /NDL /NJH /NJS /nc /ns /np";
		system(command.c_str());
	}

	else if (string(argv[1]) == "-p")
	{
		stringstream ss;
		time_t now = time(0);
		tm *ltm = localtime(&now);

		ss << std::put_time(ltm, "%d-%m-%Y %H:%M:%S");
		string date_time = ss.str();

		system(("mkdir \"" + filesystem::current_path().string() + "\\SoftwareDistribution\\RestorePoint\\" + date_time + "\"").c_str());
		command = "robocopy . \"" + filesystem::current_path().string() + "\\SoftwareDistribution\\RestorePoint\\" + date_time + "\" /XD \"" + filesystem::current_path().string() + "\\SoftwareDistribution\" /E /S /NFL /NDL /NJH /NJS /nc /ns /np";
		system(command.c_str());
	}

	else
	{
		cout << "Use -r to restore AOs" << endl;
		cout << "Use -p to create a restore point of AOs" << endl;
	}

    return 0;
}
