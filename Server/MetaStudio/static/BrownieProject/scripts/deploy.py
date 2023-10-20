from brownie import MetaStudioNFT,accounts
OPENSEA_URL = "https://testnets.opensea.io/assets/goerli/{}/{}"
sample_token_uri = "https://ipfs.io/ipfs/Qmd9MCGtdVz2miNumBHDbvj8bigSgTwnr4SbyH6DNnpWdt?filename=0-PUG.json"


def deploy_and_create():
    account = accounts.load("developeraccount")
    if list(MetaStudioNFT)==[]:
        simple_collectible = MetaStudioNFT.deploy("TestContract","TCS",{"from": account})
    else:
        simple_collectible=MetaStudioNFT[-1]
    tx = simple_collectible.safeMint("0x4FCFCebac99B81C68Ad4929Aa106ee2E0A94b989",sample_token_uri, {"from": account})
    tx.wait(1)
    print(
        f"Awesome, you can view your NFT at {OPENSEA_URL.format(simple_collectible.address,simple_collectible.tokenCounter()-1)}"
    )

    print("Please wait up to 20 minutes, and hit the refresh metadata button. ")
    return simple_collectible


def main():
    deploy_and_create()
