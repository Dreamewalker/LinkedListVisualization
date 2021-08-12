aLine 0
gBne Root, null, 3

aLine 1
Exception EMPTY_LIST

aLine 3
gNew delPtr
gMove delPtr, Rear

aLine 4
gMovePrev Rear, Rear

aLine 5
gBeq Rear, null, 4

aLine 6
pDeleteNext Rear
Jmp 3

aLine 9
gMove Root, null

aLine 11
pDeleteNext delPtr
pDeletePrev delPtr
nDelete delPtr
gDelete delPtr

aLine 12
aStd
Halt