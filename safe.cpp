// RESTORE all files from RECOVERY folder.
// Include all the important includes
#include <iostream>
#include <windows.h>
#include <string.h>

using namespace std;

// Driver code.
int main(int argc, char const *argv[])
{
	if (argv[1] == "recover") system("robocopy . Sysfail\\RECOVERY . /E /S /NFL /NDL /NJH /NJS /nc /ns /np");
	else if (argv[1] == "restore") system("robocopy . SoftwareDistribution\\RestorePoint /XD SoftwareDistribution /E /S /NFL /NDL /NJH /NJS /nc /ns /np");
	return 0;
}
