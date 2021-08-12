aLine 0
gBne Root, Rear, 3

aLine 1
Exception EMPTY_LIST

aLine 3
gNew delPtr
gMove delPtr, Rear

aLine 4
gMovePrev Rear, Rear

aLine 5
pDeleteNext Rear

aLine 6
pDeleteNext delPtr
pDeletePrev delPtr
nDelete delPtr
gDelete delPtr

aLine 7
aStd
Halt