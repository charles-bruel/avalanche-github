import base64
import diff_match_patch as dmp_module

dmp = dmp_module.diff_match_patch()

Assembly =open("Assembly-CSharp.txt.b64", "r").read() #original
MAssembly =open("ModdedAssembly-CSharp.txt.b64", "r").read() #modded(to analyse)

changeLog =open("changeLog.txt", "w") #where all pacthes are stored

patches =dmp.patch_make(Assembly, MAssembly) #finds patches
patches =dmp.patch_toText(patches) #converts patches to text
changeLog.write(patches)
print('done.')

changeLog.close()
