import { environment } from './environments/environment';


const executeOnTab = async (
  target: {
    /* Whether the script should inject into all frames within the tab. Defaults to false. This must not be true if frameIds is specified. */
    allFrames?: boolean | undefined;
    /* The IDs of specific frames to inject into. */
    frameIds?: number[] | undefined;
    /* The ID of the tab into which to inject. */
    tabId: number;
  },
  files: string[]
) => {
  chrome.scripting.executeScript({
    files,
    target,
  });
};

const findCookie = (cookies: chrome.cookies.Cookie[], cookieName: string): chrome.cookies.Cookie | null => {
  const cookie = cookies.find((cookie) => cookie.name === cookieName);
  if (cookie) return cookie;
  return null;
};

chrome.tabs.onUpdated.addListener(async (tabId, changeInfo, tab) => {
  if (!tab.url) return;

  if (tab.status === 'complete' && (tab.url.includes('fragment.com') || tab.url.includes('tonscan.org') ||  tab.url.includes('tonviewer.com')))

    await executeOnTab({ tabId, allFrames: true }, ['main.js']);
});

