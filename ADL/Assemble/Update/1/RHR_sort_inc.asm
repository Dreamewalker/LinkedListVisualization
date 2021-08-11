aLine 0
gNew basePtr
gMoveNext basePtr, Root
gNewVPtr maxNext
gNewVPtr rootNext
gMoveNext rootNext, Root

aLine 1
gNew basePrev
gMove basePrev, Root
gNew maxPtr, 1085, 800
gNew maxPrev, 1085, 860
gNew currentPtr, 1085, 920
gNew currentPrev, 1085, 980

aLine 2
gBeq basePtr, Root, 45

aLine 4
gMove maxPtr, basePtr

aLine 5
gMove maxPrev, basePrev

aLine 6
gMoveNext currentPtr, basePtr

aLine 7
gMove currentPrev, basePtr

aLine 8
gBeq currentPtr, Root, 12

aLine 9
vBge maxPtr, currentPtr, 5

aLine 10
gMove maxPrev, currentPrev

aLine 11
gMove maxPtr, currentPtr

aLine 13
gMove currentPrev, currentPtr

aLine 14
gMoveNext currentPtr, currentPtr

Jmp -12

aLine 17
gBne maxPtr, basePtr, 3

aLine 18
gMoveNext basePtr, basePtr

aLine 20
gBne basePrev, Root, 5

aLine 21
gMove basePrev, maxPtr

aLine 22
gMove Rear, maxPtr

aLine 24
gMoveNext maxNext, maxPtr
nMoveRelOut maxPtr, maxPtr, 100
pSetNext maxPrev, maxNext

aLine 25
pDeleteNext maxPtr
nMoveRelOut maxPtr, Root, 100
gMoveNext rootNext, Root
pSetNext maxPtr, rootNext

aLine 26
pSetNext Root, maxPtr
aStd

Jmp -45

aLine 28
gDelete basePrev
gDelete basePtr
gDelete maxNext
gDelete maxPtr
gDelete maxPrev
gDelete rootNext
gDelete currentPtr
gDelete currentPrev
aStd
Halt