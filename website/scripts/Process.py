import sys, csv, os, Export
import Import as Dimp
Exporter = []

mode = input('Enter mode (\'t\' to enter terminal mode and other for update): ')

if __name__ == '__main__' and mode != 't':
    class Logger(object):
        def __init__(self):
            self.terminal = sys.stdout
            self.log = open("logfile.log", "w", encoding="utf8")
       
        def write(self, message):
            self.terminal.write(message)
            self.log.write(message)  

        def flush(self):
            pass

        def close(self):
            self.log.close()

    sys.stdout = Logger()

with open('Chat.txt', encoding="utf8") as f:
    lines = f.read()

Import = lines.split("{Reactions}") #splits into a list at each part that has the text {Reactions}
index = -1
for i in Import:
    index += 1
    Import[index] = i.split("\n",3)[3] #gets rid of the first 2 lines as they're incorect for the message

index = -1
for i in Import:
    index += 1
    Import[index] = i.split("\n",2) #formating into 3 list elements

def ExtractIDLink(string):
    element = string[2]
    start = '[Jump!]('
    end = ')\n'
    return element[element.find(start)+len(start):element.rfind(end)]

def ExtractID(string):
    element = ExtractIDLink(string)
    end = '/'
    return element[-(len(element)-(element.rfind(end)+1)):]

def ExtractAuthor(string):
    element = string[2]
    end = '\n'
    return element[9:element.find(end, 9)]
    
def ExtractContent(string):
    element = string[2]
    start = ExtractAuthor(string)
    end = 'Source'
    return element[element.find(start)+len(start):element.rfind(end)]

def ExtractThumbnail(string):
    element = string[2]
    start = ')\n'
    end = '\n'
    return element[element.rfind(start)+len(start):element.rfind(end, 0, len(element)-2)]

def ExtractDownload(string):
    element = string[2]
    if element.find('wiki') != -1:
        return 'https://snowtopia-modders.fandom.com/wiki/Maps_Download'
    index = element.find('.zip')
    if index == -1:
        return ExtractIDLink(string)
    start = '\n'
    return element[element.rfind(start, 0, index):index+4]

def ExtractRating(string):
    element = string[1]
    if element.find('**') == -1:
        return 1
    start = '**'
    end = '**'
    return element[element.find(start)+len(start):element.rfind(end)]

def help():
    print('Commands: \n UpdateToken(NewToken) - overwrites to token.txt \n DelToken() - deletes token.txt \n Export.[] commands:')
    Export.help()

def UpdateToken(NewToken):
    with open('token.txt', 'w') as f:
        f.write(NewToken)
        print('New token written sucsessfully')

def DelToken():
    os.remove('token.txt')
            
if __name__ == '__main__' and mode != 't':
    print('ExtractingData...')
    for i in Import:
        Exporting = [ExtractID(i), ExtractIDLink(i), ExtractAuthor(i), ExtractContent(i).replace('\n',' '), ExtractThumbnail(i), ExtractDownload(i).replace('\n', ''), ExtractRating(i)]
        Exporter.append(Exporting)
        if Export.UpdateData(Exporting[0], Exporting[-1]) == False:
            Export.AddData(Exporting)

    #csv export stuff now for excel
    field_names= ['ID', 'ID-link', 'Author', 'Content', 'Thumbnail', 'Download', 'Rating']
    with open('DiscordExport.csv', 'w', encoding="utf8") as csvfile:
        writer = csv.writer(csvfile)
        writer.writerow(field_names)
        writer.writerows(Exporter)
    print('ExportSucsessful!')
    sys.stdout.close()
