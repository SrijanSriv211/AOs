// UPR (Update Package Restore)
// Include all the important includes
#include <iostream>
#include <windows.h>

using namespace std;
int main()
{
	system("robocopy \".\" \"Files.x72/AOs.Old\" /XD \"AOs.Old\" /E /S /NFL /NDL /NJH /NJS /nc /ns /np");
}
