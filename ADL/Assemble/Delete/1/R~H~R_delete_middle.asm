aLine 0
sInit sTemp, {0:D}
sBge sTemp, 1, 40

aLine 1
gNew delPtr
gMove delPtr, Root

aLine 2
gBne Root, null, 3

aLine 3
Exception NOT_FOUND

aLine 5
gNewVPtr rootNext
gMoveNext rootNext, Root
gBne Root, rootNext, 4

aLine 6
gMove Root, null
Jmp 16

aLine 9
gNew rearPtr
gMove rearPtr, Root
gNewVPtr rearNext
gMoveNext rearNext, rearPtr

aLine 10
gBeq rearNext, Root, 5

aLine 11
gMove rearPtr, rearNext
gMoveNext rearNext, rearNext
Jmp -5

aLine 13
gMoveNext Root, Root

aLine 14
pSetNext rearPtr, Root

aLine 16
pDeleteNext delPtr
nDelete delPtr
gDelete delPtr
gDelete rearPtr
gDelete rearNext
gDelete rootNext

aLine 17
aStd
Halt

aLine 19
gNew delPtr
gMove delPtr, Root

aLine 20
gNew prevPtr
gMove prevPtr, Root

aLine 21
sInit i, 0
sBge i, {0:D}, 12

aLine 22
gBne delPtr, null, 3

aLine 23
Exception NOT_FOUND

aLine 25
gMove prevPtr, delPtr

aLine 26
gMoveNext delPtr, delPtr

aLine 21
sInc i, 1
Jmp -11

aLine 28
gBne delPtr, null, 3

aLine 29
Exception NOT_FOUND

aLine 31
gNewVPtr delNext
gMoveNext delNext, delPtr
nMoveRelOut delPtr, delPtr, 100
pSetNext prevPtr, delNext

aLine 32
pDeleteNext delPtr
nDelete delPtr
gDelete delPtr
gDelete prevPtr
gDelete delNext

aLine 33
aStd
Halt