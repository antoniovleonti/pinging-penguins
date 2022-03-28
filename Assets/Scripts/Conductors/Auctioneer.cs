using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PriorityQueues;

public class Auctioneer : MonoBehaviour
{

    public int PlayerCount;
    public int[] CurrentBids;
    BinaryHeap<(int,int),int> bidQ;
    float RemainingSeconds = 30f;
    public RoundManager manager;
    AuctionInput input;
    AuctionUIRenderer ui;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("auctioneer created!");
        manager = gameObject.GetComponent<RoundManager>();

        ui = gameObject.GetComponent<AuctionUIRenderer>(); 
        input = gameObject.AddComponent<AuctionInput>();

        PlayerCount = input.PlayerCount;
        CurrentBids = new int[PlayerCount];
        bidQ = new BinaryHeap<(int,int),int>(PriorityQueueType.Minimum);
    }
    void OnDestroy()
    {
        Debug.Log("auctioneer destroyed!");
        // if we aren't auctioning, we don't want auction-related input
        GameObject.Destroy(input);
    }

    // Update is called once per frame
    void Update()
    {
        if (bidQ.Count > 0)
        {
            // a bid has been made; start the countdown.
            RemainingSeconds -= Time.deltaTime;
            ui.RefreshTime(RemainingSeconds);
        }
        if (RemainingSeconds <= 0) 
        {
            manager.StartProofs(bidQ);
        }
    }

    public void AddPlayerBid(int player, int bid)
    {
        int current = CurrentBids[player];
        if (current > 0 && bid >= current) return;
        // update bid
        CurrentBids[player] = bid;
        bidQ.Enqueue((player,bid),bid); // add player to queue
        ui.RefreshBids(CurrentBids);
    }
}
