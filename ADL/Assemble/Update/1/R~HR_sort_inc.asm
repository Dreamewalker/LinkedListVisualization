aLine 0
gBne Root, null, 3

aLine 1
Halt

aLine 3
nNew headNode, -1

aLine 4
nMoveRelOut headNode, Root, 150
pSetNext headNode, Root

aLine 5
pSetNext Rear, headNode

aLine 6
gMove Root, headNode
aStd

aLine 7
gNew basePtr
gMoveNext basePtr, Root
gNewVPtr maxNext
gNewVPtr rootNext
gMoveNext rootNext, Root

aLine 8
gNew basePrev
gMove basePrev, Root
gNew maxPtr, 1085, 800
gNew maxPrev, 1085, 860
gNew currentPtr, 1085, 920
gNew currentPrev, 1085, 980

aLine 9
gBeq basePtr, Root, 45

aLine 11
gMove maxPtr, basePtr

aLine 12
gMove maxPrev, basePrev

aLine 13
gMoveNext currentPtr, basePtr

aLine 14
gMove currentPrev, basePtr

aLine 15
gBeq currentPtr, Root, 12

aLine 16
vBge maxPtr, currentPtr, 5

aLine 17
gMove maxPrev, currentPrev

aLine 18
gMove maxPtr, currentPtr

aLine 20
gMove currentPrev, currentPtr

aLine 21
gMoveNext currentPtr, currentPtr

Jmp -12

aLine 24
gBne maxPtr, basePtr, 3

aLine 25
gMoveNext basePtr, basePtr

aLine 27
gBne basePrev, Root, 5

aLine 28
gMove basePrev, maxPtr

aLine 29
gMove Rear, maxPtr

aLine 31
gMoveNext maxNext, maxPtr
nMoveRelOut maxPtr, maxPtr, 100
pSetNext maxPrev, maxNext

aLine 32
pDeleteNext maxPtr
nMoveRelOut maxPtr, Root, 100
gMoveNext rootNext, Root
pSetNext maxPtr, rootNext

aLine 33
pSetNext Root, maxPtr
aStd

Jmp -45

aLine 35
gNewVPtr headNext
gMoveNext headNext, headNode
nMoveRelOut headNode, headNode, 100
pSetNext Rear, headNext

aLine 36
gMoveNext Root, headNode

aLine 37
pDeleteNext headNode
nDelete headNode
gDelete headNode

aLine 38
gDelete basePrev
gDelete basePtr
gDelete maxNext
gDelete maxPtr
gDelete maxPrev
gDelete rootNext
gDelete currentPtr
gDelete currentPrev
gDelete headNext
aStd
Halt