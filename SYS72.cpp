// SYS72: x72-Process System
// Include all the important includes
#include <iostream>
#include <windows.h>
#include <string.h>

using namespace std;
int main(int argc, char const *argv[])
{
    /*
        Root: {01lkds01-ld1s00l0-s0ls0001-we01egr1}
        Core: {1r010dkn-010ds01l-001kld01-010kld10}
        Co-SYS: {0ks011d0-0ldks10l-10d00e1v-01o01e1o}
    */

    string CmdLine = argv[1];
    if (CmdLine == "root.appdata")
        system("echo {01lkds01-ld1s00l0-s0ls0001-we01egr1}>Files.x72/Packages/appdata/Root.app");

    else if (CmdLine == "core.appdata")
        system("echo {1r010dkn-010ds01l-001kld01-010kld10}>Files.x72/Packages/appdata/Core.app");

    else if (CmdLine == "co-sys.appdata")
        system("echo {0ks011d0-0ldks10l-10d00e1v-01o01e1o}>Files.x72/Packages/appdata/Co-SYS.app");

    else
        exit(0);

    return 0;
}
