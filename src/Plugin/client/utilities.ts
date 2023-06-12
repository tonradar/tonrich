export const api = async <T>(url: string, requestSettings?: RequestInit): Promise<T> => {
  const result = await fetch(url, requestSettings);
  return result.json() as Promise<T>;
};

export const asyncForEach = async (array: any[], callback: CallableFunction) => {
  for (let index = 0; index < array.length; index++) {
    await callback(array[index], index, array);
  }
};

/**
 * Takes an HTML Element and applies the styles to it
 * @param element The target Element for the styles
 * @param styles The styles to apply
 */
export const applyStyles = (element: HTMLElement, styles: Partial<CSSStyleDeclaration>) => {
  for (const newStyle in styles) {
    // @ts-ignore
    element.style[newStyle] = styles[newStyle];
  }
};

/**
 * Easier querySelector
 * @param selector The selector to find the element
 * @param parent The parent element to search in
 * @returns The found element or null
 */
export const querySelector = <T extends HTMLElement, U extends HTMLElement>(
  selector: string,
  parent: Document | U = document
): T | null => {
  return parent.querySelector(selector);
};

/**
 * Easier querySelector
 * @param selector The selector to find the element
 * @param parent The parent element to search in
 * @returns The found element or null
 */
export const querySelectorAll = (
  selector: string,
  parent: Document | HTMLElement = document
): NodeListOf<HTMLElement> => {
  return parent.querySelectorAll(selector);
};

/**
 * Get the key of a property in an object based on it's value
 * @param object The object to look through
 * @param value The value we check based on to find the key
 * @returns The key of the value
 */
export const getObjectKeyByValue = (object: Record<string | number, unknown>, value: unknown) => {
  return Object.keys(object).find((key) => object[key] === value);
};

export const toFarsiNumber = (n: number) => {
  const farsiDigits = ['۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹'];

  // @ts-ignore
  return n.toString().replace(/\d/g, (x) => farsiDigits[x]);
};
