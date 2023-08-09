import { environment } from '../environments/environment';
import tonrich from './tonrich';
import cssExports from './main.scss';

class tonviwer extends tonrich{
  numberOfElement = 0;

  override addBadge(): void {
    this.addTonrichBadgeToTonViewer();
  }

  addTonrichBadgeToTonViewer() {
    setTimeout(() => {
      this.addBadgeElement();
    }, 1000);
  
    
    setInterval(() => {
      var numberOfCurrentElement = document.querySelectorAll('a').length;
      if (numberOfCurrentElement != this.numberOfElement) {
        this.numberOfElement = numberOfCurrentElement;
        this.addBadgeElement();
      }
    }, 1000);
  }
  
  addBadgeElement(): boolean {
    let transactions = document.querySelectorAll('.simple');

    let hasNewTransaction = false;

    for (let i = 0; i < transactions.length; i++) {
      let transaction = transactions[i] as any;

      let reactProps = transaction[Object.keys(transaction).filter((k) => k.includes('Props'))[0]];

      if (!reactProps || transaction.classList.contains('new-elm-added')) {
        continue;
      }

      hasNewTransaction = true;

      transaction.classList.add('new-elm-added');
      let tonrichTagDiv = document.createElement('div');

      tonrichTagDiv.classList.add(cssExports['tonrich-tag-tonviwer']);
      tonrichTagDiv.addEventListener('click', async (e) => {
        e.stopPropagation();
        e.preventDefault();
        let currentWalletId = this.getWalletId(transaction);
        
        this.showSite(
          `${environment.tonrichAddress}/${currentWalletId}`,
          transaction,
          cssExports['tonrich-page-tonviwer']
        );
      });
      let tonrichIcon = document.createElement('img');
      tonrichIcon.src = 'https://tonrich.app/images/fragmant-icon.svg';
      tonrichTagDiv.appendChild(tonrichIcon);
      transaction.insertBefore(tonrichTagDiv, transaction.children[0]);
      transaction.classList.add(cssExports['tonrich-target-element-tonviwer']);
    }

    return hasNewTransaction;
  }

  getWalletId(htmlElement: any): string | null {
    if (htmlElement) {
      let reactProps = htmlElement[Object.keys(htmlElement).filter((k) => k.includes('Props'))[0]];
      if (reactProps?.children?.props?.address) {
        return reactProps?.children?.props?.address.toString() ?? null;
      } else if (!!reactProps?.children && Array.isArray(reactProps?.children)) {
        var wallet = reactProps?.children?.filter(
          (f: { props: { children: { props: { address: any } } } }) => f?.props?.children?.props?.address
        );
        if (wallet && wallet.length > 0) {
          return wallet[0].props.children?.props?.address;
        }
      }
    }
  
    return null;
  }
}

export default tonviwer;