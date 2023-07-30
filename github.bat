@echo off

echo Checking status
git status

echo Adding file(s)
git add .

echo Commiting updates
git commit -m $1

echo Pushing updates
git push origin master
