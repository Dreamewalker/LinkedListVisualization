aLine 0
sInit sTemp, {0:D}
sBge sTemp, 1, 20

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
pDeletePrev Root

aLine 7
pDeleteNext delPtr
pDeletePrev delPtr
nDelete delPtr
gDelete delPtr

aLine 8
aStd
Halt


aLine 10
gNew delPtr
gMove delPtr, Root

aLine 11
sInit i, 0
sBge i, {0:D}, 10

aLine 12
gBne delPtr, null, 3

aLine 13
Exception NOT_FOUND

aLine 15
gMoveNext delPtr, delPtr

aLine 11
sInc i, 1
Jmp -9

aLine 17
gBne delPtr, null, 3

aLine 18
Exception NOT_FOUND

aLine 20
gNewVPtr delNext
gMoveNext delNext, delPtr
gNewVPtr delPrev
gMovePrev delPrev, delPtr
nMoveRel delPtr, delPtr, 0, -164.545
gBne delPtr, Rear, 4

aLine 21
gMove Rear, delPrev
Jmp 3

aLine 24
pSetPrev delNext, delPrev

aLine 26
pSetNext delPrev, delNext

aLine 27
pDeleteNext delPtr
pDeletePrev delPtr
nDelete delPtr
gDelete delPtr
gDelete delPrev
gDelete delNext

aLine 28
aStd
Halt