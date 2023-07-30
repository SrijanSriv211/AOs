@echo off

echo -> Checking status
git status

echo. & echo -> Adding file(s)
git add .

echo. & echo -> Commiting updates
git commit -m $1

echo. & echo -> Pushing updates
git push origin master
