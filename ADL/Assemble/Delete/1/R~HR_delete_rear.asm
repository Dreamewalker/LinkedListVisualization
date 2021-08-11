aLine 0
gBne Root, null, 3

aLine 1
Exception EMPTY_LIST

aLine 3
gBne Root, Rear, 10

aLine 4
nDelete Root

aLine 5
gMove Root, null

aLine 6
gMove Rear, null

aLine 7
aStd
Halt

aLine 9
gNew prevPtr
gMove prevPtr, Root
gNewVPtr nextPtr
gMoveNext nextPtr, Root

aLine 10
gBeq nextPtr, Rear, 5

aLine 11
gMove prevPtr, nextPtr
gMoveNext nextPtr, nextPtr
Jmp -5

aLine 13
gNew delPtr
gMove delPtr, Rear

aLine 14
gMove Rear, prevPtr

aLine 15
nMoveRelOut delPtr, delPtr, 100
pDeleteNext delPtr
pSetNext prevPtr, Root

aLine 16
nDelete delPtr

aLine 17
gDelete delPtr
gDelete prevPtr
gDelete nextPtr
aStd
Halt