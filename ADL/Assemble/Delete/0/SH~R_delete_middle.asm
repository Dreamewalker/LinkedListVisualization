aLine 0
gNew delPtr
gMoveNext delPtr, Root

aLine 1
gNew prevPtr
gMove prevPtr, Root

aLine 2
sInit i, 0
sBge i, {0:D}, 12

aLine 3
gBne delPtr, null, 3

aLine 4
Exception NOT_FOUND

aLine 6
gMove prevPtr, delPtr

aLine 7
gMoveNext delPtr, delPtr

aLine 2
sInc i, 1
Jmp -11

aLine 9
gBne delPtr, null, 3

aLine 10
Exception NOT_FOUND

aLine 12
gNewVPtr delNext
gMoveNext delNext, delPtr
nMoveRel delPtr, delPtr, 0, -164.545
pSetNext prevPtr, delNext

aLine 13
pDeleteNext delPtr
nDelete delPtr
gDelete prevPtr
gDelete delPtr
gDelete delNext

aLine 14
aStd
Halt