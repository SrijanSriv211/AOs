@echo off

echo -> Checking status
git status

echo. & echo ~ Adding file(s) & echo.
git add .

echo. & echo ~ Commiting updates & echo.
git commit -m $1

echo. & echo ~ Pushing updates
git push origin master
