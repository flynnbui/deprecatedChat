import { arrayBufferToBase64, base64ToArrayBuffer } from "./encryptionService";

// src/services/SignalProtocolStore.js
class SignalProtocolStore {
  constructor() {
    this.store = {};
  }

  // Generic methods
  put(key, value) {
    this.store[key] = value;
    console.log(`Stored key: ${key}`, value);
  }

  get(key, defaultValue) {
    const value = this.store[key];
    console.log(`Retrieved key: ${key}`, value);
    return value !== undefined ? value : defaultValue;
  }


  remove(key) {
    delete this.store[key];
  }

  // Identity Key Pair
  getIdentityKeyPair() {
    const serialized = this.get('identityKey');
    if (!serialized) return null;
    return {
      pubKey: base64ToArrayBuffer(serialized.pubKey),
      privKey: base64ToArrayBuffer(serialized.privKey),
    };
  }

  setIdentityKeyPair(identityKeyPair) {
    const serialized = {
      pubKey: arrayBufferToBase64(identityKeyPair.pubKey),
      privKey: arrayBufferToBase64(identityKeyPair.privKey),
    };
    this.put('identityKey', serialized);
  }


  // Registration ID
  getLocalRegistrationId() {
    return this.get('registrationId');
  }

  setLocalRegistrationId(registrationId) {
    this.put('registrationId', registrationId);
  }

  // PreKeys
  storePreKey(keyId, keyPair) {
    this.put(`preKey${keyId}`, keyPair);
  }

  loadPreKey(keyId) {
    return this.get(`preKey${keyId}`) || null;
  }

  removePreKey(keyId) {
    this.remove(`preKey${keyId}`);
  }

  // Signed PreKeys
  storeSignedPreKey(keyId, keyPair) {
    this.put(`signedPreKey${keyId}`, keyPair);
  }

  loadSignedPreKey(keyId) {
    return this.get(`signedPreKey${keyId}`) || null;
  }

  removeSignedPreKey(keyId) {
    this.remove(`signedPreKey${keyId}`);
  }

  // Sessions
  loadSession(identifier) {
    const key = `session${identifier}`;
    const value = this.get(key);
    if (value === undefined) {
      return null;
    }
    const buffer = base64ToArrayBuffer(value);
    console.log(`Loading session: ${identifier}`, buffer);
    return buffer;
  }

  storeSession(identifier, record) {
    console.log(`Storing session: ${identifier}`, record);
    const key = `session${identifier}`;
    const value = arrayBufferToBase64(record);
    this.put(key, value);
  }

  removeSession(identifier) {
    this.remove(`session${identifier}`);
  }

  // Identity Keys
  isTrustedIdentity(identifier, identityKey, direction) {
    // For simplicity, return true
    return Promise.resolve(true);
  }

  loadIdentityKey(identifier) {
    return this.get(`identityKey${identifier}`) || null;
  }

  saveIdentity(identifier, identityKey) {
    this.put(`identityKey${identifier}`, identityKey);
  }

  // Get Sub Device Sessions
  getSubDeviceSessions(identifier) {
    return Promise.resolve([]);
  }
}

export default SignalProtocolStore;
