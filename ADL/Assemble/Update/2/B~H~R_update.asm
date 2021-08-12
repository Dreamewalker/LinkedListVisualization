aLine 0
gNew updatePtr
gMove updatePtr, Root

aLine 1
gBne updatePtr, null, 3

aLine 2
Exception NOT_FOUND

aLine 4
gNewVPtr updateNext
gMoveNext updateNext, updatePtr
sInit i, 0
sBge i, {0:D}, 11

aLine 5
gBne updateNext, null, 3

aLine 6
Exception NOT_FOUND

aLine 8
gMove updatePtr, updateNext
gMoveNext updateNext, updateNext

aLine 4
sInc i, 1
Jmp -10

aLine 10
nSetValue updatePtr, {1:D}

aLine 11
gDelete updatePtr
gDelete updateNext
aStd
Halt