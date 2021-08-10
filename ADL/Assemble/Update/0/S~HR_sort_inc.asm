aLine 0
gNew basePtr
gMove basePtr, Root
gNewVPtr maxNext

aLine 1
gNew basePrev
gMove basePrev, null
gNew maxPtr, 1085, 800
gNew maxPrev, 1085, 860
gNew currentPtr, 1085, 920
gNew currentPrev, 1085, 980

aLine 2
gBeq basePtr, null, 46

aLine 4
gMove maxPtr, basePtr

aLine 5
gMove maxPrev, basePrev

aLine 6
gMoveNext currentPtr, basePtr

aLine 7
gMove currentPrev, basePtr

aLine 8
gBeq currentPtr, null, 12

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
gBne basePrev, null, 5

aLine 21
gMove basePrev, maxPtr

aLine 22
gMove Rear, maxPtr

aLine 24
gBeq maxPrev, null, 12

aLine 25
gMoveNext maxNext, maxPtr
nMoveRel maxPtr, maxPtr, 0, -164.545
pSetNext maxPrev, maxNext

aLine 26
pDeleteNext maxPtr
nMoveRel maxPtr, Root, -95, -164.545
pSetNext maxPtr, Root

aLine 27
gMove Root, maxPtr
aStd

Jmp -46

aLine 30
gDelete basePrev
gDelete basePtr
gDelete maxNext
gDelete maxPtr
gDelete maxPrev
gDelete currentPtr
gDelete currentPrev
aStd
Halt