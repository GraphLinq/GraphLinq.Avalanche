# GraphLinq.Avalanche

Plugin of the GraphLinq Engine communicating with the Avalanche Network

# Basic Blocks

Basic blocks for interacting with the Avalanche Network

## Avalanche Connector

For API and WebSocket connections we recommend using [Moralis](https://moralis.io/) or any other provider that supports API/WSS connections.

- Input
  - Url
  - Socket Url
- Output
  - Connection

### On Avalanche Block (Event)

Event triggered on each new block.

- Input
  - Avalanche Connector
- Output
  - Block

### Get Block Parameters

Converts a block into output parameters

- Input
  - Block
- Output
  - Gas Used
  - Gas Limit
  - Block Hash

### On Avalanche Transaction (Event)

Event triggered on each transaction.

- Input
  - Avalanche Connector
- Output
  - Transaction Hash

### Get Transaction Parameters

Converts a transaction into output parameters

- Input
  - Transaction
- Output
  - Value
  - From
  - To
  - Gas Price
  - Gas
  - Hash
  - Block Hash
  - Block Number
  - Nonce

# SnowTrace Blocks

Blocks for [SnowTrace](https://snowtrace.io/), an Avalanche C-Chain Explorer.

## Connector

**SnowTrace Connector** - [SnowTrace API](https://snowtrace.io/apis)

- Input
  - API Key
- Output
  - SnowTrace Connection

## Accounts

**Get AVAX Balance Single Address**

- Input
  - SnowTrace Connection
  - Wallet Address
- Output
  - Wallet Balance

## Stats

- Input
  - SnowTrace Connector
- Output
  - Total Supply

## Tokens

**Get ERC-20 Balance For Contract**

- Input
  - SnowTrace Connector
  - Contract Address
  - Token Address
- Output
  - Account Balance

**Get ERC-20 Token Supply**

- Input
  - SnowTrace Connector
  - Contract Address
- Output
  - Total Supply

# Avascan

Blocks for [Avascan.info](https://avascan.info/)

Avascan does not require an API Key so there isn't a Connector Block. These blocks have no inputs and return the latest results.

## Burn

**Get Burned Fees**

- Output
  - X
  - C
  - Xc

## Stats

**Get Global Stats**

- Output
  - Blockchains
  - Validators
  - Staking Ratio
  - Staking Rewards
  - Price
  - Market Cap By Circulating Supply
  - Market Cap By Total Supply
  - Circulating Supply
  - Last Transactions 24hrs
  - Last Average TPS 24hrs
  - Assets and Tokens
  - Burned Since Launch

**Get Staking Stats**

- Output
  - Total Validator
  - Total Delegation
  - Total Stake
  - Total Validation Stake
  - Total Delegated Stake
  - Staking Reward
  - Staking Ratio

## Supply

**Get Supply**

- Output
  - Genesis Unlock
  - Staking Rewards
  - Last Update
  - Circulating Supply
  - Total Supply

## GraphQL

Not Implemented. Please see [Nodes/Avascan/.graphql](https://github.com/GraphLinq/GraphLinq.Avalanche/tree/main/Nodes/Avascan/.graphql) for a full list of possibilities. Will add these upon request.
