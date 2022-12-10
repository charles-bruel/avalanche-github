import os

if os.path.exists('token.txt'):
    with open('token.txt', 'r') as f:
        token = f.read()
else:
    token = input('Enter a token: ')

os.system(f'dotnet DicordChatExporter.CLI/DiscordChatExporter.Cli.dll export -t "{token}" -c 939905115505180682 -o Chat.txt -f PlainText')
#Gets a list of all messages from maps-pinboard

with open('Chat.txt', 'r', encoding="utf8") as f:
    lines = f.readlines()

for i in range(19):
    lines.pop(0)
    
lines[0] = '\n'

for i in range(6):
    lines.pop(len(lines)-1)

lines[-1:] = '\n'

with open('Chat.txt', 'w', encoding="utf8") as f:
    lines = f.writelines(lines)

print('Import Sucsessful!')
