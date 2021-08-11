aLine 0
gBne Root, null, 3

aLine 1
Halt

aLine 3
gNew rearPtr
gMove rearPtr, Root
gNewVPtr rearNext
gMoveNext rearNext, rearPtr

aLine 4
gBeq rearNext, Root, 5

aLine 5
gMove rearPtr, rearNext
gMoveNext rearNext, rearNext
Jmp -5

aLine 7
nNew headNode, -1

aLine 8
nMoveRelOut headNode, Root, 150
pSetNext headNode, Root

aLine 9
pSetNext rearPtr, headNode

aLine 10
gMove Root, headNode
aStd

aLine 11
gNew basePtr
gMoveNext basePtr, Root
gNewVPtr minNext
gNewVPtr rootNext
gMoveNext rootNext, Root

aLine 12
gNew basePrev
gMove basePrev, Root
gNew minPtr, 1085, 800
gNew minPrev, 1085, 860
gNew currentPtr, 1085, 920
gNew currentPrev, 1085, 980

aLine 13
gBeq basePtr, Root, 45

aLine 15
gMove minPtr, basePtr

aLine 16
gMove minPrev, basePrev

aLine 17
gMoveNext currentPtr, basePtr

aLine 18
gMove currentPrev, basePtr

aLine 19
gBeq currentPtr, Root, 12

aLine 20
vBle minPtr, currentPtr, 5

aLine 21
gMove minPrev, currentPrev

aLine 22
gMove minPtr, currentPtr

aLine 24
gMove currentPrev, currentPtr

aLine 25
gMoveNext currentPtr, currentPtr

Jmp -12

aLine 28
gBne minPtr, basePtr, 3

aLine 29
gMoveNext basePtr, basePtr

aLine 31
gBne basePrev, Root, 5

aLine 32
gMove basePrev, minPtr

aLine 33
gMove rearPtr, minPtr

aLine 35
gMoveNext minNext, minPtr
nMoveRelOut minPtr, minPtr, 100
pSetNext minPrev, minNext

aLine 36
pDeleteNext minPtr
nMoveRelOut minPtr, Root, 100
gMoveNext rootNext, Root
pSetNext minPtr, rootNext

aLine 37
pSetNext Root, minPtr
aStd

Jmp -45

aLine 39
gNewVPtr headNext
gMoveNext headNext, headNode
nMoveRelOut headNode, headNode, 100
pSetNext rearPtr, headNext

aLine 40
gMoveNext Root, headNode

aLine 41
pDeleteNext headNode
nDelete headNode
gDelete headNode

aLine 42
gDelete basePrev
gDelete basePtr
gDelete minNext
gDelete minPtr
gDelete minPrev
gDelete rootNext
gDelete currentPtr
gDelete currentPrev
gDelete headNext
gDelete rearNext
gDelete rearPtr
aStd
Halt