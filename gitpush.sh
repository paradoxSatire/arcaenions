#!/bin/bash
git add .
echo -n "Enter a commit message: "
read answer
git commit -m "$answer"

expect -c "
spawn git push https://github.com/paradoxSatire/arcaenions
expect -nocase \"Username for 'https://github.com':\" {send \"paradoxSatire\r\"; exp_continue}
expect -nocase \"Password for 'https://paradoxSatire@github.com':\" {send \"F@lling2\r\"; interact}
"
