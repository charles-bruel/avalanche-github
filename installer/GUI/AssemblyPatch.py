import base64
import imp
from locale import currency
import diff_match_patch as dmp_module
import os

def Patch(changeLogDir, NewAssembly):
    dmp = dmp_module.diff_match_patch() #a patching library by google https://github.com/google/diff-match-patch

    Assembly = open("Assembly-CSharp.txt.b64", "r").read()  #original assembly (converted to Base64)
    NAssembly = open(NewAssembly, "w") #new modded assembly

    changeLog = open(changeLogDir, 'r').read()

    patches = dmp.patch_fromText(changeLog) #gets pacthes
    Final = dmp.patch_apply(patches, Assembly)[0] #converts Assembly into new modded assembly

    NAssembly.write(Final)
    NAssembly.close()