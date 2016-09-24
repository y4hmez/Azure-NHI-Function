let digitSumModulus digitSum = let y = (11 - digitSum)
                               match y with                                
                                        | 10 -> 0 
                                        | _ -> y

let GetDigitSum (nhi:string) =
    
    let x = nhi.ToCharArray()
    let d = dict [('A',1);('B',2) ;('C',3) ;('D',4);('F',6);('G',7);('E',5) ;('H',8);('J',9);('K',10);('L',11);('M',12);('N',13);('Q',15);('R',16);('P',14);('S',17);('T',18);('U',19);('V',20);('W',21);('X',22);('Y',23);('Z',24)]    

    let sum = x |> Seq.take(6)
                |> Seq.mapi (fun i x ->   match i < 3 with                                  
                                                | true  -> d.[x] * (7 - i)                                                                                                                                
                                                | false -> (int (System.Char.GetNumericValue x)) * (7 - i)
                        )
                |> Seq.sum
                |> fun i -> i % 11
        
    match sum with
        | 0 -> None
        | _ -> Some(sum |> digitSumModulus)
                          

let IsValidNHI2 (nhi:string) =

    try

        let chkDigit = (int (System.Char.GetNumericValue (nhi.ToCharArray()).[6]))
        let digitSum = GetDigitSum nhi

        match digitSum with
        | Some value -> (value = chkDigit)
        | None -> false

    with
    | :? System.Collections.Generic.KeyNotFoundException -> (printfn "Key Lookup failure"; false)
    | _ ->   false

        
let GetBaseNhiByIndex (idx:int) =

    let numSet = [| for i in 0 .. 999 do yield (i.ToString()).PadLeft(3,'0')|]

    let chrs = [for c in 'A' .. 'Z' do if (c <> 'I' && c <> 'O') then yield c]
    
    let alpha =  [| for i in 0 .. 23 do   
                    for j in 0 .. 23 do 
                    for k in 0 .. 23 do
                    yield System.String.Concat(chrs.[i],chrs.[j],chrs.[k])
    |]

    let nmbrIdx = idx % 1000
    let alphaIdx = (idx - nmbrIdx) / 1000

    System.String.Concat(alpha.[alphaIdx],numSet.[nmbrIdx])
    
//min idx = 0;  max idx 13823999    
let GetFullNHIByIndex (idx:int) = 
    let nhi = GetBaseNhiByIndex idx    
    let chkDigit = GetDigitSum nhi

    match chkDigit with
    | Some c -> Some(System.String.Concat(nhi,c))
    | None -> None
    

let MakeNhiGenerator =
         
    let curIdx = ref 0

    let rec NhiGenerator() =
        let nhiOption = GetFullNHIByIndex curIdx.Value
        curIdx := !curIdx + 1

        match nhiOption with 
        | Some nhi -> nhi
        | None -> NhiGenerator()
        
    NhiGenerator
    
let Generator =  MakeNhiGenerator 