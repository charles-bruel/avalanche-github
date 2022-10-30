import binascii

def Validate(path):
    count = 0
    scale = 16 # equals to hexadecimal

    with open(path, 'rb') as f:
        hexdata = binascii.hexlify(f.read())
        binary = bin(int(hexdata, scale))[2:]

    for i in str(binary):
        count += int(i)

    return count

