aLine 0
sInit sTemp, {0:D}
sBge sTemp, 1, 17

aLine 1
gNew delPtr
gMove delPtr, Root

aLine 2
gBne Root, null, 3

aLine 3
Exception NOT_FOUND

aLine 5
gMoveNext Root, Root

aLine 6
pDeleteNext delPtr
nDelete delPtr
gDelete delPtr

aLine 7
aStd
Halt

aLine 9
gNew delPtr
gMove delPtr, Root

aLine 10
gNew prevPtr
gMove prevPtr, Root

aLine 11
sInit i, 0
sBge i, {0:D}, 12

aLine 12
gBne delPtr, null, 3

aLine 13
Exception NOT_FOUND

aLine 15
gMove prevPtr, delPtr

aLine 16
gMoveNext delPtr, delPtr

aLine 11
sInc i, 1
Jmp -11

aLine 18
gBne delPtr, null, 3

aLine 19
Exception NOT_FOUND

aLine 21
gNewVPtr delNext
gMoveNext delNext, delPtr
nMoveRel delPtr, delPtr, 0, -164.545
pSetNext prevPtr, delNext

aLine 22
pDeleteNext delPtr
nDelete delPtr
gDelete delPtr
gDelete prevPtr
gDelete delNext

aLine 23
aStd
Halt