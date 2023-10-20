// SPDX-License-Identifier: MIT
pragma solidity 0.6.6;

import "@openzeppelin/contracts/token/ERC721/ERC721.sol";
import "@openzeppelin/contracts/access/Ownable.sol";
import "@openzeppelin/contracts/utils/Counters.sol";

contract MetaStudioNFT is ERC721, Ownable {

    uint256 public tokenCounter;

    constructor(string memory _name, string memory _symbol)  public ERC721(_name, _symbol) {
        tokenCounter = 0;
    }

    function safeMint(address to,string memory tokenURI) public onlyOwner {
        uint256 tokenId = tokenCounter;
        tokenCounter = tokenCounter + 1;
        _safeMint(to, tokenId);
        _setTokenURI(tokenId, tokenURI);
    }
    function retrieve() public view returns (uint256){
        return tokenCounter;
    }
    function retrieve_uri(uint256 tok_id) public view returns (string memory){
        return tokenURI(tok_id);
    }

    // The following functions are overrides required by Solidity.
}