# DOHYO
Dohyo is a Discord bot for casual gambling on sumo wrestling. In our server, we use the fantastic Twitch channel [MidnightSumo](https://www.twitch.tv/MidnightSumo) to watch the games.

This bot was mostly to see what the experience of using the Discord.NET package was like (spoiler: it was great!), but feel free to use all or any part of it as a reference for creating your own bot.  

## Prerequisites
Dohyo requires you to provide a token (`DISCORD_TOKEN`) and a path to where the sqlite database file is to be stored (`DB_URI`) as environment variables.

## Run this locally
Build and run `dohyo` with Docker:
```sh
git clone https://github.com/richardtatum/dohyo.git dohyo
cd dohyo
docker build -t dohyo .
docker run -p 8080:8080 -e DISCORD_TOKEN='valid-bot-token-here' -e DB_URI='path/to/sqlite.db' dohyo:latest
```
## Usage
Once added, Dohyo listens for slash commands in any channel. These commands should be automatically populated by Discord into the command pop-up.

### Commands
Dohyo provides the following slash commands out of the box:

#### /balance
The `balance` command provides the users current balance available for betting. By default, every user starts with ¥1,000.

![image](https://github.com/richardtatum/dohyo/assets/46816684/95d92605-7d05-46d3-ab2e-6190db235c6d)

#### /fight
The `fight` command starts a new fight, notifying users and opening the bets to everyone. NOTE: Only an admin can start a fight!

![image](https://github.com/richardtatum/dohyo/assets/46816684/3856a8fb-6720-46e5-9d83-52faf0291ff6)

#### /bet
The `bet` command allows the user to pick a side (left or right) and bet an amount. The amount bet must be less than their current balance for it to work

![image](https://github.com/richardtatum/dohyo/assets/46816684/15afc88f-39c1-4efe-ad66-974f8391d572)


#### /end
Once everyone has bet, the admin can use the `end` command, passing the winning side as an argument. This brings the currently open fight to a close and distribute the winnings accordingly.

![image](https://github.com/richardtatum/dohyo/assets/46816684/21ebbe2e-1e96-4c34-b2e9-48cdd5666e0a)


### Bonus Commands
These final two commands are mostly made in jest. One person in our server asked why we could tip the adjudicator of the fights (lovingly nicknamed 'The Wizard') after betting, similar to how you would tip the dealer at a poker table. Thus these commands were created.


#### /tip
The `tip` command allows any user to gift any amount of their current balance to The Wizard.

![image](https://github.com/richardtatum/dohyo/assets/46816684/76032bc4-782b-464d-b422-e149bbc3d1ed)


#### /wizard
The `wizard` command shows the current balance of the adjudicator, and just how generous your friends have been!

![image](https://github.com/richardtatum/dohyo/assets/46816684/c716dbaa-1cde-45e5-a7b5-fb0ee15626ce)
