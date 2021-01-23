// UPR (Update Package Restore)
// Include all the important includes
#include <iostream>
#include <windows.h>

using namespace std;

int main(int argc, char const *argv[])
{
	/*
	REMEMBER TO BUILD AND MOVE UPR.exe TO UpdatePackages FOLDER!!!
	IT IS IMPORTANT!!!!!!!
	*/

	system("robocopy \".\" \"UpdatePackages/AOs.old\" /XD \"UpdatePackages\"  /E /S");
	return 0;
}
