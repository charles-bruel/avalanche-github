import binascii
import codecs

Converted = open("Assembly-CSharp.txt.b64", "w")
ModdedAssembly = open("ModdedAssembly-CSharp.txt.b64", "w")

with open('Assembly-CSharp.dll', 'rb') as f:
    hexdata = binascii.hexlify(f.read())
    hex = hexdata
    b64data = codecs.encode(codecs.decode(hex, 'hex'), 'base64').decode()
    
    Converted.write(b64data)

with open('ModdedAssembly-CSharp.dll', 'rb') as f:
    hexdata = binascii.hexlify(f.read())
    hex = hexdata
    b64data = codecs.encode(codecs.decode(hex, 'hex'), 'base64').decode()
    
    ModdedAssembly.write(b64data)
