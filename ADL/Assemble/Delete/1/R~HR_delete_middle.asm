aLine 0
sInit sTemp, {0:D}
sBge sTemp, 1, 29

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
gBne Root, rootNext, 6

aLine 6
gMove Root, null

aLine 7
gMove Rear, null
Jmp 5

aLine 10
gMoveNext Root, Root

aLine 11
pSetNext Rear, Root

aLine 13
pDeleteNext delPtr
nDelete delPtr
gDelete delPtr
gDelete rootNext

aLine 14
aStd
Halt

aLine 16
gNew delPtr
gMove delPtr, Root

aLine 17
gNew prevPtr
gMove prevPtr, Root

aLine 18
sInit i, 0
sBge i, {0:D}, 12

aLine 19
gBne delPtr, null, 3

aLine 20
Exception NOT_FOUND

aLine 22
gMove prevPtr, delPtr

aLine 23
gMoveNext delPtr, delPtr

aLine 18
sInc i, 1
Jmp -11

aLine 25
gBne delPtr, null, 3

aLine 26
Exception NOT_FOUND

aLine 28
gBne delPtr, Rear, 3

aLine 29
gMove Rear, prevPtr

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