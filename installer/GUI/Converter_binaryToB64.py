import binascii
import codecs

def Convert(AssemblyPath):
    Converted = open("Assembly-CSharp.txt.b64", "w")

    with open(AssemblyPath, 'rb') as f:
        hexdata = binascii.hexlify(f.read())
        hex = hexdata
        b64data = codecs.encode(codecs.decode(hex, 'hex'), 'base64').decode()
        
        Converted.write(b64data)
